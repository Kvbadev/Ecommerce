<script lang="ts">
    //TODO: make them slider smaller
import { faAngleLeft, faAngleRight} from "@fortawesome/free-solid-svg-icons";
import {fade} from 'svelte/transition';

import Fa from "svelte-fa";
import type Product from "../../../Models/product";
export let product: Product;
export let maxPhotos = 3;
const photos = [product.mainPhoto, ...product.photos];
let index = 0;

</script>

<div class="container">
    <div class="slider">
        <span class="button left" on:click={() => index = --index%(0-maxPhotos)}>
            <Fa icon={faAngleLeft} size={"7x"} color={'rgba(0, 0, 0, 0.7)'}/>
        </span>

        <span in:fade class="big-image">
            <img src={photos.at(index)} alt="Product photos"/>
        </span>

        <span class="button right" on:click={() => index = ++index%maxPhotos}>
            <Fa icon={faAngleRight} size={"7x"} color={'rgba(0, 0, 0, 0.7)'}/>
        </span>  
    </div>
    <div class="others">
        {#each photos as photo, i}
            <div class="image {photos.at(index) === photo ? 'main':''}">
                <img on:click={() => {index=i}} src={photo} alt="Product" />
            </div>
        {/each}
    </div>
</div>

<style>
    .big-image {
        overflow: hidden;
        border: black 0.15rem solid;
        width: 52rem;
        height: 52rem;
    }                
    .button {
        cursor: pointer;
    }
    .others {
        width: 82%;
        height: 7.5vw;
        display: flex;
        align-items: center;
        justify-content: center;
        background-color: rgba(255, 255, 240, 0.7)
    }
    .image {
        cursor: pointer;
        border: 0.2rem gray solid;
        margin: 0 2rem;
        width: 11rem;
        height: 11rem;
    }
    .main {
        border: 0.3rem red solid;
    }
    .others img {
        width: 100%;
        height: 100%;
    }
    .container{
        padding: 1rem;
        grid-area: slider;
        width: 90rem;
        display: flex;
        flex-direction: column;
        justify-content: space-evenly;
        align-items: center
    }
    .slider {
        width: 90%;
        display: flex;
        align-items: center;
        justify-content: space-evenly;
    }
    .slider img {
        width: 100%;
        height: 100%;
    }

</style>