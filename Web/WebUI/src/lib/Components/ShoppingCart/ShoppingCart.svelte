<script lang="ts">
import { agent } from '../../Utils/agent';


import {shoppingCart, removeShoppingCart} from '../../Stores/shoppingCartStore';

import {products, userProfile} from '../../Stores/stores';

import Loader from '../Common/Loader.svelte';
import { link } from 'svelte-spa-router';
import Modal from '../Common/Modal.svelte';

let showModal = false;

async function clearCart() {
   removeShoppingCart(); 
}

function toggleModal() {
    showModal = !showModal;
}

function buyProducts() {

}


</script>

<div class="all">

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

        {#if $userProfile}
        <a href="/Buy" use:link><button on:click={buyProducts}>Buy products</button></a> 
        {:else}
        <a href="/Account/Login" use:link><button on:click={buyProducts}>Buy products</button></a>
        {/if}

        <button on:click={clearCart}>Clear the cart</button>
    {:else}
        <h1>You have not added any products to your cart</h1>
    {/if}
</div>
{/if}
</div>

<style>
    .all {
        width: 100%;
        height: calc(100vh - 4.2vw);
    }
    .container *{
        margin: 0.1rem 1rem ;
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