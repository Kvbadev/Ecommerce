<script lang="ts">

import {removeShoppingCart, saveLocalCart} from '../../Stores/ShoppingCartExtensions';

import {products, userProfile, shoppingCart} from '../../Stores/stores';

import Loader from '../Common/Loader.svelte';
import { link } from 'svelte-spa-router';
import CartProduct from './CartProduct.svelte';
  import { onDestroy, onMount } from 'svelte';
  import type Cart from 'src/lib/Models/cart';
  import { get } from 'svelte/store';

async function clearCart() {
   removeShoppingCart(); 
}

let changes: Cart | null = null;

onMount(async () => {
    
})

onDestroy(async () => {
    if( changes !== null && 
        JSON.stringify(changes) !== JSON.stringify($shoppingCart)){ 
            if($shoppingCart?.items.length === 0){
                await clearCart();
            } else {
                await saveLocalCart();
            }
    }
})

//cannot use onmount because cart'd not be initialized
$: if(changes === null && $shoppingCart !== null){
    changes = {...get(shoppingCart)};
}

</script>
<div class="container">
{#if !$shoppingCart}
    <Loader />
{:else}

{#if $shoppingCart.items.length}
<h1>My Shopping Cart</h1>
        <div class="items">
            {#each $shoppingCart.items as cartitem}
                <CartProduct prod={cartitem} />
            {/each}
        </div>
        <div class="buttons">
            {#if $userProfile}
            <a href="/Buy" use:link class="buy-btn">
                <button>Buy products</button>
            </a> 
            {:else}
            <a href="/Account/Login" use:link class="buy-btn">
                <button >Buy products</button>
            </a>
            {/if}
            <button on:click={clearCart}>Clear the cart</button>
        </div>
    {:else}
        <h1>You have not added any products to your cart</h1>
    {/if}
{/if}
</div>

<style>
    .buttons {
        display: flex;
        flex-direction: row;
        justify-content: center;
    }
    .container h1 {
        font-size: 4rem;
    }
    .buttons button {
        margin: 2rem;
        display: block;
        width: 25rem;
        border: none;
        border-radius: 2rem;
        background-color: black;
        font-size: 3.0rem;
        font-family: 'Raleway';
        color: white;
        height: 7rem;
        cursor: pointer;
    }
    .buy-btn{
        width: auto;
        height: auto;
        margin: 2rem;
    }
    .buy-btn button {
        color: black;
        margin: 0;
        border: 0.2rem black solid;
        border-radius: 2rem;
        background-color: white;
    }
    .items {
        display: flex;
        justify-content: flex-start;
        padding: 1.5rem 0;
        align-items: center;
        flex-direction: column;
        overflow: auto;
        margin: 0 auto;
        width: 85%;
        height: 50vh;
        border: 0.1rem black solid;
        border-radius: 1rem;
    }
    h1 {
        padding: 3rem;
        font-family: 'Raleway';
        text-align: center;
    }
    .container {
        width: 100%;
        height: calc(100vh - 4.2vw);
        display: flex;
        flex-direction: column;
        justify-content: space-evenly;
    }
    a {
        width: auto;
        display: inline-block;
    }

</style>