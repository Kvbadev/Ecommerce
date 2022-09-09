<script lang="ts">
import type Product from "src/lib/Models/product";


    export let product: Product;
</script>

<div class="container">
    <h1>{product.description}</h1>
</div>

<style>
    h1 {
        font-family: 'Anek Telugu', sans-serif;
        font-size: 3.5rem;
        width: 60%;
        max-height: 100%;
    }
    .container {
        overflow: scroll;
        grid-area: params;
        height: 50rem;
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: aliceblue;
        /* overflow: hidden; */
    }
</style>