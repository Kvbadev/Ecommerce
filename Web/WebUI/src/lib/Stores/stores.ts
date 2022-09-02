import { writable } from "svelte/store";
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