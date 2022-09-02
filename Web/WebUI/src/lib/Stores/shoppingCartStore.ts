import { get, writable } from "svelte/store";
import type { CartItem } from "../Models/cart";
import type Cart from "../Models/cart";

export const shoppingCart = writable(null as Cart|null);

export const updateShoppingCart = (item: CartItem) => {
    shoppingCart.update((v) => {
        let isInCart = false;
        v.items = v.items.map((v, i) => {
            if(v.id === item.id){
                v.quantity+=item.quantity;
                isInCart = true;
            };
            return v;
        });
        if(!isInCart){
            v.items.push(item);
        }

        v.count = v.items.reduce((acc, items) => acc+items.quantity, 0);
        v.sum = v.items.reduce((acc, items) => acc+(items.price*items.quantity), 0).toFixed(2);
        return v;
    });
    localStorage.setItem("cart", JSON.stringify(get(shoppingCart)));
}
export const initShoppingCart = (cart?: Cart) => {
    if(cart){
        shoppingCart.set(cart);
    } else {
    shoppingCart.set({items: new Array<CartItem>, count: 0, sum: 0});
    }
    localStorage.setItem("cart", JSON.stringify(get(shoppingCart)));
}

export function removeShoppingCart() {
    localStorage.removeItem("cart");
    shoppingCart.set(null);
    initShoppingCart();
}