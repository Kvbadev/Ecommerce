import { writable } from "svelte/store";
import type Product from "../Models/product";
import type Profile from "../Models/profile";
import type User from "../Models/user";

export const products = writable(new Array<Product>());

export const jwtToken = writable('');

//TODO: check if user exist on refresh
export const userProfile = writable({} as User | Profile);