<script lang="ts">

import { push } from "svelte-spa-router";
import { modifyCart } from "../../../Stores/ShoppingCartExtensions";
import {oneTimeProduct} from "../../../Stores/stores";


export let params = {} as any;
export let quantity: number;

const OnCart = () => {
    modifyCart({quantity: quantity, id: params.id});
}

const buyNow = () => {
    oneTimeProduct.set({id: params.id, quantity: quantity});
    push('/buyNow');
}

</script>

<div class="container">
    
    <button class="buy-btn" on:click={buyNow}><span>Buy Now</span></button>
    <button on:click={OnCart} class="cart-btn"><span>Add to cart</span></button>
</div>

<style>
    .container {
        display: flex;
        flex-direction: row;
    }
    button {
        cursor: pointer;
        font-family: Raleway;
        border: black 0.4rem solid;
        width: 50%;
        height: 8rem;
        background-color: transparent;
        font-size: 3rem;
        text-align: center;
        transition: position 0.5s ease-in-out;
    }
    button:active {
        background-color: white;
    }
    span::after {
        content: '»'; 
        position: absolute;
        right: -2rem;
        top: 0;
        /* left: 1.5rem; */
        opacity: 0;
        transition: 0.5s;
    }
    button:hover span:after {
        opacity: 1;
        right: 0;
    }
    span {
        display: inline-block;
        position: relative;
        transition: 0.5s; 
    }
    button:hover span{
        padding-right: 2.5rem;
    }
    div {
        display: flex;
        align-items: center;
        justify-content: space-around;
    }
    .cart-btn {
        background-color: black;
    }
    .cart-btn span{
        color: white;
    }

</style>