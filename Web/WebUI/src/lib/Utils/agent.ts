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
import type Client from "../Models/client";

const apiUrl = "https://localhost:5000/api";

async function getCart(url: string): Promise<Cart | null> {
    const cart = await authFetch<Cart>(url, 'GET', null);
    if(cart?.items?.length === 0) return null;

    return cart;
}

async function resendFetch(url: string, method, headers, body) {
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
                const check = await agent.Account.refreshTokens();
                if(check) {
                    const jwt = localStorage.getItem("jwt");
                    jwt ? headers["Authorization"] = `Bearer ${jwt}` : null;

                    resp = await resendFetch(url, method, headers, body);
                }

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
    try{
        if(access && refresh){
            const res = await fetch(url, {
                method: 'PATCH',
                body: JSON.stringify({accessToken:access, refreshToken:refresh}),
                headers: {
                    'Content-Type': 'application/json',
                }
            }).then(async res => {
                if(!res.ok) throw res.status;
                return await res.text();
            }).then(text => JSON.parse(text));

            refreshToken.set(res.refreshToken);
            jwtToken.set(res.accessToken);
            localStorage.setItem("jwt", res.accessToken);
            localStorage.setItem("refresh", res.refreshToken);
            return true;
        }
    } catch(err) {
        console.error(err);
        return false;
    }
}

export const agent = {
    Products: {
        getAll: () => authFetch<Array<Product>>(apiUrl+"/products", 'GET', null),
        getOne: (id: string) => authFetch<Product>(apiUrl+`/products/${id}`, 'GET', null),
        remove: (id: string) => authFetch<string>(apiUrl+`/products/${id}`, 'DELETE', null),
        edit: (id: string, prod: Partial<Product>) => authFetch<string>(apiUrl+`/products/${id}`, 'PATCH', prod)
    },
    Account: {
        signUp: (user: User) => authFetch<[number, string]>(apiUrl+"/Account/register", 'POST', user, true),
        logIn: async (user: User) => authFetch<[number, string]>(apiUrl+"/Account/login", 'POST', user, true),
        getProfile: () => authFetch<null | Profile>(apiUrl+'/Account/profile', 'GET', null),
        getTransactions: () => authFetch<Array<Transaction>>(apiUrl+'/Account/transactions', 'GET', null),
        updateProfile: (profile: Partial<Profile>) => authFetch<string>(apiUrl+'/Account/profile', "PATCH", profile),
        refreshTokens: () => refresh(apiUrl+'/token/refresh'),
        isAdmin: () => authFetch<boolean>(apiUrl+'/Account/isAdmin', 'GET',null),
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
    },
    Admin: {
        getClients: () => authFetch<Client[]>(apiUrl+'/Account/clients', 'GET', null),
        updateRoles: (roles: Array<string>, user: Client) => authFetch<string>(apiUrl+`/Roles/update/${user.username}`, 'PATCH', roles)
    }
}