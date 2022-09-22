<script lang="ts">
import { faRemove } from "@fortawesome/free-solid-svg-icons";
import type { CartItem } from "src/lib/Models/cart";
import { modifyCart } from "../../Stores/ShoppingCartExtensions";
import Fa from "svelte-fa";
import {products} from '../../Stores/stores';
import {shoppingCart} from '../../Stores/stores';
import QuantityInput from "../Products/Details/QuantityInput.svelte";
  import { link } from "svelte-spa-router";

export let prod: CartItem;
let product;

$: {
    if($shoppingCart) //if statement used to update on shoppingcart's change
        product = $products.find(x => x.id === prod.id)
};

async function removeItem() {
    await modifyCart({...prod,quantity: -prod.quantity}, true);
}


</script>

<div class="container">
    <div id="photo">
        <img alt="Product" class="main-photo" src="{product.mainPhoto}"/>
    </div>
    <div class="main-info" id="main-info">
        <p><a href={`/product/${product.id}`} use:link>{product.name}</a></p>
        <p class="little-price">{product.price}$</p>
    </div>
    <div class="quantity">
        <QuantityInput bind:quantity={prod.quantity} ModifyCart productID={prod.id}/>
    </div>
    <div class="remove">
        <span on:click={removeItem}>
            <Fa icon={faRemove} size={'3.5x'}/>
        </span>
    </div>
    <div class="price">
        <p>{(product.price*prod.quantity).toFixed(2)}$</p>
    </div>
</div>

<style>
    .container {
        display: flex;
        align-items: center;
        justify-content: space-evenly;
        flex-direction: row;
        border: 0.2rem black solid;
        border-radius: 1.5rem;
        margin: 1rem;
        padding: 0 0.5rem;
        width: 90%;
        height: auto;
    }
    .container div {
        width: 20%;
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
        font-size: 2rem;
        color: gray;
    }
    
    .container * {
    padding: 0.2rem;
    }
    img {
        width: 10rem;
        height: 10rem;
        border-radius: 2rem;
    }
    .remove span{
        cursor: pointer;
    }
    p {
        font-family: 'Roboto Slab';
        font-size: 3.5rem;
    }
</style>