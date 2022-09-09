import type Cart from "../Models/cart";
import type { CartItem } from "../Models/cart";
import type Product from "../Models/product";
import { toast } from '@zerodevx/svelte-toast';
import type Profile from "../Models/profile";
import type  User  from "../Models/user";

const apiUrl = "https://localhost:5000/api";

const postProducts = async (url: string, product: Product) => {
    let response;
    try{
        response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(product)
        });
    } catch (error) {
        console.log(error);
    }
    return response.json();
}

async function getCart(url: string): Promise<Cart | null> {
    const cart = await authFetch<Array<CartItem>>(url, 'GET', null);
    
    if(cart?.length === 0) return null;

    let newCart: Cart = {items: new Array<CartItem>, count: 0, sum: 0};
    for(const item of cart){
        newCart.items.push(item);
        newCart.count+=item.quantity;
        newCart.sum+=item.quantity*item.price;
    }

    return newCart;
}

async function authFetch<T>(url: string, method:'POST'|'GET'|'PUT'|'DELETE'|'PATCH', body?: any, validation:boolean=false): Promise<T>{
    const jwt = localStorage.getItem("jwt");

    const headers = {};
    
    method.startsWith('P') ? headers["Content-Type"] = 'application/json' : null; //starts with P so post patch and put
    jwt ? headers["Authorization"] = `Bearer ${jwt}` : null;

    try{
        const response = await fetch(url, {
            method: method,
            headers: {...headers},
            body: body ? JSON.stringify(body) : null
        }).then(async (response) => {
            if(!response.ok){
                toast.push(response.statusText.length > 60 ? 'Server could not handle your request' : response.statusText);
            }
            if(validation){
                return [response.status, await response.text()] as T;
            }
            return response.status !== 204 ? 
            response.json().then(data => data as T) : 
            null;
        });
        return response;
    } catch (e) {
        toast.push(e);
        console.log(e);
    }
}

export const agent = {
    Products: {
        getAll: () => authFetch<Array<Product>>(apiUrl+"/products", 'GET', null),
        getOne: (id: string) => authFetch<Product>(apiUrl+"/products/"+id, 'GET', null),
        post: async (product: Product) => await postProducts(apiUrl+"/products", product)
    },
    Account: {
        signUp: (user: User) => authFetch<[number, string]>(apiUrl+"/Account/register", 'POST', user, true),
        logIn: async (user: User) => authFetch<[number, string]>(apiUrl+"/Account/login", 'POST', user, true),
        getProfile: () => authFetch<null | Profile>(apiUrl+'/Account/profile', 'GET', null),
    },
    ShoppingCart: {
        GetCart: () => getCart(apiUrl+"/ShoppingCart"),
        addItem: (item: CartItem) => authFetch<string>(apiUrl+"/ShoppingCart/add", 'PATCH', item),
        removeItem: (item: CartItem) => authFetch<string>(apiUrl+"/ShoppingCart/delete", 'PATCH', null),
        clearCart: () => authFetch<string>(apiUrl+"/ShoppingCart", 'DELETE', null)
    }
}