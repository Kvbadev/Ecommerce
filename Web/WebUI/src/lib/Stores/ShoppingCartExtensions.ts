import { get, writable } from "svelte/store";
import type { CartItem } from "../Models/cart";
import type Cart from "../Models/cart";
import { agent } from "../Utils/agent";
import { products, shoppingCart, userProfile } from "./stores";


export const addToCart = async (item: CartItem) => {
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
        v.sum = parseFloat(v.items.reduce((acc, items) => acc+(get(products).find(x => x.id === items.id)?.price*items.quantity), 0).toFixed(2));
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

export const removeFromCart = async (item: CartItem) => {
    const product = get(products).find(x => x.id === item.id);
    if(get(userProfile)?.username){
        await agent.ShoppingCart.removeItem(item);
    }
    shoppingCart.update((v) => {

        const itemInCart = v.items.find(x => x.id === item.id);
        itemInCart.quantity -= 1;

        if(itemInCart.quantity === 0){
            v.items = v.items.filter(x => x.id !== item.id);
        }
        v.count -= 1;
        v.sum -= product.price;
        return v;
    });
    localStorage.setItem("cart", JSON.stringify(get(shoppingCart)));
}

export async function removeShoppingCart() {
    if(get(userProfile)?.username){
        await agent.ShoppingCart.clearCart();
    }
    localStorage.removeItem("cart");
    initShoppingCart();
}