import { get, writable } from "svelte/store";
import type Cart from "../Models/cart";
import type {CartItem} from "../Models/cart";
import type Product from "../Models/product";
import type Profile from "../Models/profile";
import { agent } from "../Utils/agent";

export const products = writable(null as Array<Product>|null);

export async function getProducts() {
    // setTimeout(async () => (products.set(await agent.Products.get())), 1000);
    products.set(await agent.Products.getAll());
}

export const jwtToken = writable(null as string|null);

export const userProfile = writable(null as Profile|null);

//TODO: move stores to separate files
export const shoppingCart = writable(null as Cart|null);

export const updateShoppingCart = (item: CartItem) => {
    shoppingCart.update((v) => {
        v.items.push(item);
        v.count = v.items.reduce((acc, items) => acc+items.quantity, 0);
        v.sum = v.items.reduce((acc, items) => acc+items.price, 0);
        return v;
    });
    localStorage.setItem("cart", JSON.stringify(get(shoppingCart)));
}