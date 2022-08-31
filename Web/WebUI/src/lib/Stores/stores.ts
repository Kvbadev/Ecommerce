import { writable } from "svelte/store";
import type Product from "../Models/product";
import type Profile from "../Models/profile";

export const products = writable(null as Array<Product>|null);

export const jwtToken = writable(null as string|null);

export const userProfile = writable(null as Profile|null);