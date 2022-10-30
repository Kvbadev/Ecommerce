<script lang="ts">
  import { faClose } from "@fortawesome/free-solid-svg-icons";
import Fa from "svelte-fa";
  import { fade } from "svelte/transition";

    export let src = "";
</script>

<div class="container" out:fade on:click={() => src=""}>
    <span on:click={() => src=""}><Fa icon={faClose} color="white" size="3x"/></span>
    <img src={src} in:fade  alt="product" on:click={(e) => e.stopImmediatePropagation()}/>
</div>

<style>
    .container {
        position: absolute;
        width: 100vw;
        height: 100vh;
        display: grid;
        place-items: center;
        backdrop-filter: brightness(30%);
        cursor: pointer;
    }

    span {
        position: absolute;
        top: 6.5%;
        left: 66.5%;
        z-index: 2;
        cursor: pointer;
    }
    img {
        cursor: default;
        width: 35%;
        height: 80%;
        /* width: 70%;
        height: 70%; */
    }
</style>