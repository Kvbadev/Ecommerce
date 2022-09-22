import { writable } from "svelte/store";
import type Cart from "../Models/cart";
import type Product from "../Models/product";
import type Profile from "../Models/profile";

export const products = writable(null as Array<Product>|null);

export const jwtToken = writable(null as String|null);

export const userProfile = writable(null as Profile|null);

const oneTimeProduct = writable(JSON.parse(localStorage.getItem('oneTimeProduct' || ''))); //if we want not to buy entire cart
oneTimeProduct.subscribe(val => localStorage.setItem('oneTimeProduct', JSON.stringify(val ?? '')));
export {oneTimeProduct};

export const shoppingCart = writable(null as Cart|null);