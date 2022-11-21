import type Product from "./product";

export interface CartItem{
    quantity: number;
    product: Product;
}
export default interface Cart {
    items: Array<CartItem>;
    count: number;
    sum: number;
}

