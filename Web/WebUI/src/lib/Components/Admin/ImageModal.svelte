<script lang="ts">
  import { faClose } from "@fortawesome/free-solid-svg-icons";
import Fa from "svelte-fa";
  import { fade } from "svelte/transition";

    export let src = "";
</script>

<div class="container" out:fade on:click={() => src=""}>
    <div>
        <span on:click={() => src=""}><Fa icon={faClose} color="white" size="3x"/></span>
        <img src={src} in:fade  alt="product" on:click={(e) => e.stopImmediatePropagation()}/>
    </div>
</div>

<style>
    .container {
        position: fixed;
        width: 100vw;
        z-index: 100;
        height: 100vh;
        display: grid;
        place-items: center;
        backdrop-filter: brightness(30%);
        cursor: pointer;
    }

    .container > div {
        position: relative;
    }

    span {
        position: absolute;
        top: -4.5%;
        left: 98%;
        z-index: 2;
        cursor: pointer;
    }
    img {
        cursor: default;
        width: 35vw;
        height: 30vw;
        /* width: 70%;
        height: 70%; */
    }
</style>