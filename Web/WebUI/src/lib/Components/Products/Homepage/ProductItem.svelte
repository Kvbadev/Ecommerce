<script lang="ts">
import type Product from "src/lib/Models/product";
import ItemPhoto from "../../Products/Homepage/ItemPhoto.svelte";

    export let product:Product;

    const img_width = "300px";
    const img_height = "300px";
    const max_desc_len = 150;

    const photoProps = {
        src: `/PlaceholderPhotos/${product.name.replaceAll(' ', '_').toLowerCase()}${(Math.random()+1*2).toFixed(0)}.png`,
        width: img_width,
        height: img_height,
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
    @import url('https://fonts.googleapis.com/css2?family=Raleway&display=swap');

    li {
        cursor: pointer;
        font-size: 20px;
        font-family: "Raleway";
        list-style-type: none;
        width: 30%;
        height: 32vw;
        overflow: hidden;
        margin: 5% 2%;
        display: flex;
       /* From https://css.glass */
        background: rgba(255, 255, 255, 0.37);
        border-radius: 16px;
        box-shadow: 0 4px 30px rgba(0, 0, 0, 0.38);
        backdrop-filter: blur(20px);
        -webkit-backdrop-filter: blur(10px);
        -moz-backdrop-filter: blur(10px); 
        border: 1px solid rgba(0, 0, 0, 0.3);

        transition: 0.5s ease-in-out;
    }

    li:hover {
        box-shadow: 0 4px 30px rgba(0,0,0,0.7);
        background-color: white;
    }

    a {
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: space-evenly;
    }

    li p {
        margin-top: 10px;
    }
    
    .name {
        
        font-size: 2vw;
        padding: 10px;
        font-weight: 700;
    }
    .description {
        font-size: 1.15vw;
        width: 80%;
    }
    .price {
        font-size: 30px;
    }
</style>