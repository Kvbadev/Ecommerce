import { writable } from "svelte/store";
import type Cart from "../Models/cart";
import type { CartItem } from "../Models/cart";
import type Product from "../Models/product";
import type Profile from "../Models/profile";

export const products = writable(null as Array<Product>|null);

export const jwtToken = writable(null as String|null);

export const userProfile = writable(null as Profile|null);

export const oneTimeProduct = writable(null as CartItem|null); //if we want not to buy entire cart

export const shoppingCart = writable(null as Cart|null);