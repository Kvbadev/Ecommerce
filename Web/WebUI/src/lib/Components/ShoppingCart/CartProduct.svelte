<script lang="ts">

import { faQuestionCircle, faRemove } from "@fortawesome/free-solid-svg-icons";
import { modifyCart } from "../../Stores/ShoppingCartExtensions";
import Fa from "svelte-fa";
import QuantityInput from "../Products/Details/QuantityInput.svelte";
import { link } from "svelte-spa-router";
import { onMount } from "svelte";

export let prod; //CartItem
export let simplified = false;
export let size = 62.5;

onMount(async () => {
})
async function removeItem() {
    await modifyCart({...prod,quantity: -prod.quantity}, true);
}


</script>

<div class="container" style={`font-size: ${size}%`}>
    {#if prod }
        <div id="count">
            <h4>{prod.quantity}x</h4>
        </div>
        <div id="photo">
            <img alt="Product" class="main-photo" src="{prod.product?.photos.at(0)}"/>
        </div>
        <div class="main-info" id="main-info">
            <p><a href={`/product/${prod.product?.id}`} use:link>{prod.product?.name}</a></p>
            {#if !simplified}
                <p class="little-price">{prod.product?.price}$</p>
            {/if}
        </div>
        {#if !simplified}
        <div class="quantity">
            <QuantityInput bind:quantity={prod.quantity} ModifyCart product={prod}/>
        </div>
        <div class="remove">
            <span on:click={removeItem}>
                <Fa icon={faRemove} size={'3.5x'}/>
            </span>
        </div>
        {/if}
        <div class="price">
            <p>{(prod.product.price*prod.quantity).toFixed(2)}$</p>
        </div>
    {:else}
    <li class="item">
        <h4>Products have been removed 
            <span on:click={()=>{}}><Fa icon={faQuestionCircle}/></span>
        </h4>
    </li>
    {/if}
</div>
<!-- {/if} -->

<style>
    .container {
        display: flex;
        align-items: center;
        justify-content: space-evenly;
        flex-direction: row;
        border: 0.2em black solid;
        border-radius: 1.5em;
        margin: 1em;
        padding: 0 0.5em;
        width: 90%;
        height: auto;
    }
    .container div {
        width: 20%;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    #count {
        width: 5%;
        height: auto;
        font-size: 3em;
        font-family: 'Roboto slab';
    }

    h4 {
        height: auto;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    #main-info {
        width: 30%;
        align-items: center;
        flex-direction: column;
    }

    .little-price {
        font-size: 2em;
        color: gray;
    }
    
    .container * {
    padding: 0.2em;
    }
    img {
        width: 10em;
        height: 10em;
        border-radius: 2em;
    }
    .remove span{
        cursor: pointer;
    }
    p {
        font-family: 'Roboto Slab';
        font-size: 3em;
        text-align: center;
    }
</style>