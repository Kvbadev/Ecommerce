import { writable } from "svelte/store";
import type Product from "../Models/product";

export const products = writable(new Array<Product>());

export const multiplier = writable(1);