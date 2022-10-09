import type { CartItem } from "./cart";

export default interface Transaction {
    id: string;
    price: number;
    products: Array<CartItem>;
    issuedAt: Date;
    Success: boolean;
}