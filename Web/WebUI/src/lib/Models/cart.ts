export interface CartItem{
    quantity: number;
    price: number;
    id: string;
}
export default interface Cart {
    items: Array<CartItem>;
    count: number;
    sum: number;
}

