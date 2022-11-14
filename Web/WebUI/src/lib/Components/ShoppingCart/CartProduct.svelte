<script lang="ts">
import { faQuestionCircle, faRemove } from "@fortawesome/free-solid-svg-icons";
import type { CartItem } from "src/lib/Models/cart";
import { modifyCart } from "../../Stores/ShoppingCartExtensions";
import Fa from "svelte-fa";
import {products} from '../../Stores/stores';
import {shoppingCart} from '../../Stores/stores';
import QuantityInput from "../Products/Details/QuantityInput.svelte";
  import { link } from "svelte-spa-router";

export let prod: CartItem;
export let simplified = false;
export let size = 62.5;
let product;

$: {
    if($shoppingCart && $products){ //if statement used to update on shoppingcart's change
        product = $products.find(x => x.id === prod.id)
        console.log(product);
    }
};

async function removeItem() {
    await modifyCart({...prod,quantity: -prod.quantity}, true);
}


</script>
{#if $products} <!--Weird bug when opening profile without this if-->

<div class="container" style={`font-size: ${size}%`}>
    {#if product }
        <div id="count">
            <h4>{prod.quantity}x</h4>
        </div>
        <div id="photo">
            <img alt="Product" class="main-photo" src="{product?.photos.at(0)}"/>
        </div>
        <div class="main-info" id="main-info">
            <p><a href={`/product/${product?.id}`} use:link>{product?.name}</a></p>
            {#if !simplified}
                <p class="little-price">{product?.price}$</p>
            {/if}
        </div>
        {#if !simplified}
        <div class="quantity">
            <QuantityInput bind:quantity={prod.quantity} ModifyCart productID={prod.id}/>
        </div>
        <div class="remove">
            <span on:click={removeItem}>
                <Fa icon={faRemove} size={'3.5x'}/>
            </span>
        </div>
        {/if}
        <div class="price">
            <p>{(product.price*prod.quantity).toFixed(2)}$</p>
        </div>
    {:else}
    <li class="item">
        <h4>Products have been removed 
            <span on:click={()=>{}}><Fa icon={faQuestionCircle}/></span>
        </h4>
    </li>
    {/if}
</div>
{/if}

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