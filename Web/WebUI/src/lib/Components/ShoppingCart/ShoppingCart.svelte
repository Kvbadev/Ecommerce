<script lang="ts">
import { agent } from '../../Utils/agent';


import {shoppingCart, removeShoppingCart} from '../../Stores/shoppingCartStore';

import {products} from '../../Stores/stores';

import Loader from '../Common/Loader.svelte';
import { link } from 'svelte-spa-router';

async function clearCart() {
   removeShoppingCart(); 
}

function buyProducts() {

}


</script>

{#if !$shoppingCart || !$products}
    <Loader />
{:else}
<div class="container">
    {#if $shoppingCart.items.length}
        <h1>{$shoppingCart.sum.toFixed(2)}$</h1>
        {#each $shoppingCart.items.map(v => {
            // console.log(v);
            const item = $products.find(x => x.id===v.id);
            // console.log(item);
            const sum = v.price * v.quantity;
            return {item: item, quantity: v.quantity, sum: sum}
            }) as {item, quantity, sum}}

            <h3>{item?.name} [{quantity}] - {sum.toFixed(2)}$</h3>
        {/each}
        <button on:click={buyProducts}><a href="/Buy" use:link>Buy products</a></button>
        <button on:click={clearCart}>Clear the cart</button>
    {:else}
        <h1>You have not added any products to your cart</h1>
    {/if}
</div>
{/if}

<style>
    .container *{
        margin: 1rem;
    }
    button {
        display: block;
        width: 15rem;
        border: none;
        background-color: rgb(184, 184, 184);
        height: 5rem;
        cursor: pointer;
    }
</style>