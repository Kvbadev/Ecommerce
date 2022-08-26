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
        font-size: calc(1vw + 1vh);
        width: 60%;
        position: relative;
        top: 7rem;
        left: 3rem;
    }
    .container {
        grid-area: params;
        height: 40rem;
        /* overflow: hidden; */
    }
</style>