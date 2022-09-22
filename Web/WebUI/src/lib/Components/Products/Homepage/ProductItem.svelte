<script lang="ts">
import type Product from "src/lib/Models/product";
import ItemPhoto from "../../Products/Homepage/ItemPhoto.svelte";

    export let product:Product;

    const max_desc_len = 150;

    const photoProps = {
        src: product.mainPhoto,
        alt: product.name
    }
</script>

<li name={product.id}>

    <a href={`/#/product/${product.id}`}>
    <ItemPhoto {...photoProps}/>
    <p class="name">{product.name}</p>
    <p class="description">
        {#if product.description.length < max_desc_len}
            {product.description}
        {:else}
            {product.description.slice(0, max_desc_len)}...
        {/if}
    </p>
    <p class="price">{product.price.toFixed(2)} $</p>
    </a>
</li>

<style>
    li {
        cursor: pointer;
        font-size: 2rem;
        font-family: "Raleway";
        list-style-type: none;
        width: 30%;
        height: 32vw;
        overflow: hidden;
        margin: 5% 2%;
        display: flex;
       /* From https://css.glass */
        background: rgba(255, 255, 255, 0.37);
        background-color: white;
        border-radius: 1.6rem;
        /* box-shadow: 0 0.4rem 3rem rgba(0, 0, 0, 0.38); */
        box-shadow: 0px 1px 18px -11px rgba(66, 68, 90, 1);
        backdrop-filter: blur(2rem);
        -webkit-backdrop-filter: blur(1rem);
        -moz-backdrop-filter: blur(1rem); 
        border: 0.1rem solid rgba(0, 0, 0, 0.3);

        transition: 0.5s ease-in-out;
    }

    li:hover {
        box-shadow: 0px 3px 18px 3px rgba(66, 68, 90, 1);
    }

    a {
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: space-evenly;
    }
    
    .name {
        
        font-size: 2vw;
        padding: 1rem;
        font-weight: 700;
    }
    .description {
        font-size: 1.25vw;
        width: 80%;
    }
    .price {
        font-size: 1.5vw;
    }
</style>