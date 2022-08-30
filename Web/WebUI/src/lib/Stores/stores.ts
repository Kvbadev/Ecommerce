import { writable } from "svelte/store";
import type Product from "../Models/product";
import type User from "../Models/user";

export const products = writable(new Array<Product>());

export const jwtToken = writable(new String);

//TODO: check if user exist on refresh
export const userProfile = writable({} as User);