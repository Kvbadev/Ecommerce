import type Cart from "../Models/cart";
import type { CartItem } from "../Models/cart";
import type Product from "../Models/product";
import { toast } from '@zerodevx/svelte-toast';
import type Profile from "../Models/profile";
import type  User  from "../Models/user";
import type Transaction from "../Models/transaction";
import type AuthResponse from "../Models/authResponse";
import { jwtToken, refreshToken } from "../Stores/stores";
import { get } from "svelte/store";

const apiUrl = "https://localhost:5000/api";

async function getProducts(url: string): Promise<Array<Product>> {
    let prods = await authFetch<Array<Product>>(apiUrl+"/products", 'GET', null);
    prods = prods.map((v,i) => {
        v.mainPhoto = `/PlaceholderPhotos/${v.name.replaceAll(' ', '_')
            .toLowerCase()}1.png`;

        v.photos = Array.from(Array(4)).slice(2).map((n,i) => {
            return `/PlaceholderPhotos/${v.name.replaceAll(' ', '_')
        .toLowerCase()}${i+2}.png`;
        });

        return v;
    })
    return prods;
}

async function getCart(url: string): Promise<Cart | null> {
    const cart = await authFetch<Cart>(url, 'GET', null);
    if(cart?.items?.length === 0) return null;

    return cart;
}

async function resendFetch(url: string, method, headers, body) {
    await agent.Account.refreshTokens();
    return await fetch(url, {method: method, headers: {...headers}, body: body !== null ? JSON.stringify(body):null});
}

async function authFetch<T>(url: string, method:'POST'|'GET'|'PUT'|'DELETE'|'PATCH', body?: any, validation:boolean=false): Promise<T>{
    const jwt = localStorage.getItem("jwt");

    const headers = {};
    
    method.startsWith('P') ? headers["Content-Type"] = 'application/json' : null; //starts with P so post patch and put
    jwt ? headers["Authorization"] = `Bearer ${jwt}` : null;

    const response = await fetch(url, {
        method: method,
        headers: {...headers},
        body: body !== null ? JSON.stringify(body) : null
    }).then(async (resp) => {
        if(!resp.ok){
            if(resp.status === 401 && !validation){
                console.log('401');
                await agent.Account.refreshTokens();

                const jwt = localStorage.getItem("jwt");
                jwt ? headers["Authorization"] = `Bearer ${jwt}` : null;

                resp = await resendFetch(url, method, headers, body);

            } else if(!validation) {
                toast.push(resp.statusText.length > 60 ? 'Server could not handle your request' : resp.statusText);
            }
        }
        if(validation){
            return [resp.status, await resp.text()] as T;
        }
        const contentType = resp.headers.get('content-type');
        const text = await resp?.text();
        if(text.length && contentType.indexOf('application/json') !== -1 ){
            return JSON.parse(text) as T;
        } else if(text.length) {
            return text as T;
        }
        return null;
    });
    return response;

}

const refresh = async (url: string) => {
    const access = get(jwtToken) ?? localStorage.getItem("jwt");
    const refresh = get(refreshToken) ?? localStorage.getItem("refresh");
    if(access && refresh){
        const res = await fetch(url, {
            method: 'PATCH',
            body: JSON.stringify({accessToken:access, refreshToken:refresh}),
            headers: {
                'Content-Type': 'application/json',
            }
        }).then(async res => {
            return await res.text();
        }).then(text => JSON.parse(text))
        .catch(err => console.log(err));

        refreshToken.set(res.refreshToken);
        jwtToken.set(res.accessToken);
        localStorage.setItem("jwt", res.accessToken);
        localStorage.setItem("refresh", res.refreshToken);
    }
}

export const agent = {
    Products: {
        getAll: () => getProducts(apiUrl+"/products"),
        getOne: (id: string) => authFetch<Product>(apiUrl+`/products/${id}`, 'GET', null),
    },
    Account: {
        signUp: (user: User) => authFetch<[number, string]>(apiUrl+"/Account/register", 'POST', user, true),
        logIn: async (user: User) => authFetch<[number, string]>(apiUrl+"/Account/login", 'POST', user, true),
        getProfile: () => authFetch<null | Profile>(apiUrl+'/Account/profile', 'GET', null),
        getTransactions: () => authFetch<Array<Transaction>>(apiUrl+'/Account/transactions', 'GET', null),
        updateProfile: (profile: Partial<Profile>) => authFetch<string>(apiUrl+'/Account/profile', "PATCH", profile),
        refreshTokens: () => refresh(apiUrl+'/token/refresh'),
    },
    ShoppingCart: {
        GetCart: () => getCart(apiUrl+"/ShoppingCart"),
        updateCart: (item: CartItem) => authFetch<string>(apiUrl+"/ShoppingCart/update", 'PATCH', item),
        clearCart: () => authFetch<string>(apiUrl+"/ShoppingCart", 'DELETE', null),
        setCart: (cart: Cart) => authFetch<string>(apiUrl+"/ShoppingCart", 'POST', cart)
    }, 
    PaymentGateway: {
        getToken: () => authFetch<string>(apiUrl+"/Payment/token", 'GET', null),

        getPrice: (oneTimeProd) => authFetch<string>(apiUrl+"/Payment/price", 'POST', oneTimeProd ?? {}),

        buyCart: (nonce: string, deviceData: string) => 
        authFetch<string>(apiUrl+`/Payment/buy?nonce=${nonce}`, 'POST', deviceData),

        buyProduct: (nonce: string, deviceData: string, product: CartItem) => {
        return authFetch<string>(
        apiUrl+`/Payment/buy?nonce=${nonce}&id=${product.id}&quantity=${product.quantity}`,
        'POST', deviceData)
        }
    }
}