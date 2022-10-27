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
        width: 85%;
        max-height: 100%;
    }
    .container {
        overflow: hidden;
        grid-area: params;
        height: calc(56vh - 4.2vw);
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 1rem 0;
        margin: 0 1rem 1rem 0;
        margin-top: 5%;
        border: solid 0.2rem black;
    }
</style>