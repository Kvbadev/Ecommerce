import { get, writable } from "svelte/store";
import type { CartItem } from "../Models/cart";
import type Cart from "../Models/cart";
import { agent } from "../Utils/agent";
import { userProfile } from "./stores";

export const shoppingCart = writable(null as Cart|null);

export const updateShoppingCart = async (item: CartItem) => {
    //TODO: figure out when is localstorage of cart used
    if(get(userProfile)?.username){
        await agent.ShoppingCart.addItem(item);
    }
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
        v.sum = parseFloat(v.items.reduce((acc, items) => acc+(items.price*items.quantity), 0).toFixed(2));
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

export async function removeShoppingCart() {
    if(get(userProfile)?.username){
        await agent.ShoppingCart.clearCart();
    }
    localStorage.removeItem("cart");
    initShoppingCart();
}