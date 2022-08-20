<script lang="ts">
import type Product from "src/lib/Models/product";
import ItemPhoto from "./ItemPhoto.svelte";
    export let product:Product;

    const img_width = "300px";
    const img_height = "300px";
    const max_desc_len = 150;

    const photoProps = {
        src: `public/PlaceholderPhotos/${product.name.replaceAll(' ', '_').toLowerCase()}1.png`,
        width: img_width,
        height: img_height,
        alt: product.name
    }
</script>

<li name={product.id}>
    <ItemPhoto {...photoProps}/>
    <p class="name">{product.name}</p>
    <p class="description">
        {#if product.description.length < max_desc_len}
            {product.description}
        {:else}
            {product.description.slice(0, max_desc_len)}...
        {/if}
    </p>
    <p class="price">{product.price} $</p>
</li>

<style>
    @import url('https://fonts.googleapis.com/css2?family=Raleway&display=swap');

    li {
        font-size: 20px;
        font-family: "Raleway";
        list-style-type: none;
        width: 30%;
        height: 38vw;
        overflow: hidden;
        padding: 2%;
        margin: 5%;
        display: flex;
        flex-direction: column;
        align-items: center;
        /* justify-content: space-between; */

        background: rgba(216, 88, 24, 1.0);
        background: -webkit-linear-gradient(top left, rgba(216, 88, 24, 1.0), rgba(249, 209, 77, 1.0));
        background: -moz-linear-gradient(top left, rgba(216, 88, 24, 1.0), rgba(249, 209, 77, 1.0));
        background: linear-gradient(to bottom right, rgba(216, 88, 24, 0.5), rgba(249, 209, 77, 0.5));
    }

    .name {
        font-size: 2vw;
        padding: 10px;
        font-weight: 700;
    }
    .description {
        margin-top: 20px;
        font-size: 1vw;
        width: 50%;
    }
    .price {
        margin-top: auto;
    }
</style>