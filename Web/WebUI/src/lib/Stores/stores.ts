import { push } from "svelte-spa-router";
import { get, writable } from "svelte/store";
import type AuthResponse from "../Models/authResponse";
import type Cart from "../Models/cart";
import type Product from "../Models/product";
import type Profile from "../Models/profile";
import { agent } from "../Utils/agent";
import { initShoppingCart } from "./ShoppingCartExtensions";

//Products
export const products = writable(null as Array<Product>|null);

export const initProducts = async () => {
    products.set(await agent.Products.getAll());
}

//JWT token
export const jwtToken = writable(null as string|null);

export const initUser = async (res: AuthResponse, type: 'Login'|'Signup') => {
    
    //set jwt in localstorage and in stores
    jwtToken.set(res.accessToken);
    localStorage.setItem("jwt", res.accessToken);

    refreshToken.set(res.refreshToken);
    localStorage.setItem("refresh", res.refreshToken);

    userProfile.set(await agent.Account.getProfile());

    if(get(shoppingCart).items.length && type === 'Login'){
        return true;
    }
    //if registering then persist a cart
    else if(get(shoppingCart).items.length && type === "Signup"){
        await agent.ShoppingCart.setCart(get(shoppingCart))
        push('/');

    } else {
        initShoppingCart(await agent.ShoppingCart.GetCart());
        push('/');
    }
}

export const refreshToken = writable(null as string|null);

export const userProfile = writable(null as Profile|null);

//OneTimeProduct
const oneTimeProduct = writable(JSON.parse(localStorage.getItem('oneTimeProduct' || ''))||null); //if we want not to buy entire cart
oneTimeProduct.subscribe(val => localStorage.setItem('oneTimeProduct', JSON.stringify(val ?? '')));
export {oneTimeProduct};

export const shoppingCart = writable(null as Cart|null);