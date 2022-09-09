<script lang="ts">
import type Product from "src/lib/Models/product";
import BuyButtons from "./BuyButtons.svelte";
import QuantityInput from "./QuantityInput.svelte";

    export let product: Product;

    export let multiplier: number = 1;

    export let params;

</script>

<div class="container">
    <span class="h1">
        <h1>{product.name}</h1>
    </span>
    <span class="h2">
        <h2 data-quantity={`${multiplier} ${multiplier == 1 ? "product":"products"}`}>{(product.price * multiplier).toFixed(2)}$</h2>
    </span>
    <!-- <div class="buy-buttons"></div> -->
    <BuyButtons price={product.price} params={params} quantity={multiplier}/>
    <QuantityInput bind:quantity={multiplier}/>
</div>

<style>
    @import url('https://fonts.googleapis.com/css2?family=Raleway&display=swap');
    .container {
        grid-area: price;
        display: grid;
        /* grid-auto-columns: 0.5fr; */
        grid-auto-rows: 14rem;
        grid-template-columns: 3fr 2fr;
        /* grid-template-rows: 50% 1fr; */ 
    }
    .h1 {
        grid-column: 1/2;
        font-family: 'Rubik';
        font-size: 2.5rem;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .h2 {
        background-color: aliceblue;
        font-family: 'Rubik';
        font-size: 2.5rem;
        display: grid;
        justify-content: center;
        align-content: center;
    }
    h2::after {
        content: attr(data-quantity);
        font-size: 1.2rem;
        font-style: italic;
        color: gray;
        display: block;
        text-align: center;
    }
</style>