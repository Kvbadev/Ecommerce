<script lang="ts">
import { faAngleLeft, faAngleRight} from "@fortawesome/free-solid-svg-icons";
import {fade} from 'svelte/transition';

import Fa from "svelte-fa";
import type Product from "../../../Models/product";
export let product: Product;
const photos = [product.mainPhoto, ...product.photos];
export let maxPhotos = photos.length;
let index = 0;

const onChange = (newIndex) => {
    if(newIndex < 0) newIndex = maxPhotos-1;
    index = newIndex;
}

</script>

<div class="container">
    <div class="slider">
        <span class="button left" on:click={() => onChange(index-1)}>
            <Fa icon={faAngleLeft} size={"5x"} color={'rgba(0, 0, 0, 0.7)'}/>
        </span>

        <span in:fade class="big-image">
            {#each photos as photo, i}
                <img src={photo} alt={`Product ${photo}`} 
                    style={`left: ${(((i-index)+maxPhotos)%maxPhotos)*100}%;`}
                /> 
            {/each}
        </span>

        <span class="button right" on:click={() => onChange((index+1)%maxPhotos)}>
            <Fa icon={faAngleRight} size={"5x"} color={'rgba(0, 0, 0, 0.7)'}/>
        </span>  
    </div>
    <div class="others">
        {#each photos as photo, i}
            <div class="image {photos.at(index) === photo ? 'main':''}">
                <img on:click={() => {index=i}} src={photo} alt="Product"/>
            </div>
        {/each}
    </div>
</div>

<style>
    .big-image {
        overflow: hidden;
        position: relative;
        background-color: white;
        /* border: black 0.15rem solid; */
        width: 30vw;
        height: 30vw;
    }                
    .big-image img {
        width: 100%;
        height: 100%;
        position: absolute;
        transition: 500ms all ease-out;
    }
    .button {
        cursor: pointer;
    }
    .others {
        height: 7.5vw;
        display: flex;
        align-items: center;
        justify-content: center;
        background-color: rgba(255, 255, 240, 0.7)
    }
    .image {
        cursor: pointer;
        border: 0.3rem gray solid;
        margin: 0 2rem;
        width: 90%;
        height: 90%;
        transition: all 500ms;
    }
    .main {
        border: 0.5rem rgb(217, 249, 15) solid;
    }
    .others img {
        width: 100%;
        height: 100%;
    }
    .container{
        box-sizing: border-box;
        padding: 1rem;
        grid-area: slider;
        width: 50vw;
        min-width: 25rem;
        height: auto;
        /* background-color: brown; */
        display: flex;
        flex-direction: column;
        justify-content: space-evenly;
        align-items: center
    }
    .slider {
        width: 90%;
        height: 80%;
        display: flex;
        align-items: center;
        justify-content: space-evenly;
    }


</style>