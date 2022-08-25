<script lang="ts">
import type Product from "src/lib/Models/product";
import BuyButtons from "./BuyButtons.svelte";
import QuantityInput from "./QuantityInput.svelte";

    export let product: Product;
</script>

<div class="container">
    <span class="h1">
        <h1>{product.name}</h1>
    </span>
    <span class="h2">
        <h2>{product.price}$</h2>
    </span>
    <BuyButtons />
    <QuantityInput />
</div>

<style>
    @import url('https://fonts.googleapis.com/css2?family=Raleway&display=swap');
    .container {
        
        grid-area: price;
        display: grid;
        grid-template-columns: 1fr 25%;
        grid-template-rows: 50% 1fr;
    }
    .h1 {
        font-family: 'Rubik';
        font-size: 4rem;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .h2 {
        background-color: aliceblue;
        font-family: 'Rubik';
        font-size: 4.5rem;
        display: flex;
        align-items: center;
        justify-content:center;
    }
</style>