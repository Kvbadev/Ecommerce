<script lang="ts">
import type Product from "src/lib/Models/product";


    export let product: Product;
</script>

<div class="container">
    <h1>{product.name}</h1>
    <h2>{product.price}</h2>
</div>

<style>
    .container {
        background-color: white;
        grid-area: price;
        /* height: 20vh; */
    }
</style>