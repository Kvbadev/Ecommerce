<script lang="ts">
import { faAngleLeft, faAngleRight, faArrowAltCircleLeft } from "@fortawesome/free-solid-svg-icons";
import {fade, fly} from 'svelte/transition';

import Fa from "svelte-fa";


    export let product;
    export let maxPhotos = 3;
    let smallPhotoSize = "2"

    let src = Array.from({length: 3}).map((v, i) => {
        return `/PlaceholderPhotos/${product.name.replaceAll(' ', '_').toLowerCase()}${i+1}.png`
    });

    let selectedPhoto = src[0];

    function changePhoto(forward: boolean) {
        let tmpsrc;
        if(forward){
            tmpsrc = src[(src.indexOf(selectedPhoto)+1)%maxPhotos];
        } else {
            const helper = src.indexOf(selectedPhoto)-1;
            tmpsrc = src[(helper == -1 ? maxPhotos-1 : helper)];
        }
        selectedPhoto = tmpsrc;
    }
</script>

<div class="container">
    <div class="slider">
        <span class="button left" on:click={() => changePhoto(false)}>
            <Fa icon={faAngleLeft} size={"7x"} color={'rgba(0, 0, 0, 0.7)'}/>
        </span>

        <span in:fade class="big-image">
            <img src={selectedPhoto} alt="Product photos"/>
        </span>

        <span class="button right" on:click={() => changePhoto(true)}>
            <Fa icon={faAngleRight} size={"7x"} color={'rgba(0, 0, 0, 0.7)'}/>
        </span>  
    </div>
    <div class="others">
        {#each src as photo}
            <div class="image {selectedPhoto === photo ? 'main':''}">
                <img on:click={() => {selectedPhoto = photo}} src={photo} width={smallPhotoSize} height={smallPhotoSize} alt="Product" />
            </div>
        {/each}
    </div>
</div>

<style>
    .big-image {
        overflow: hidden;
        border: black 0.15rem solid;
        width: 33vw;
        height: 33vw;
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
        width: 6.5vw;
        height: 6.5vw;
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
        width: 50vw;
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