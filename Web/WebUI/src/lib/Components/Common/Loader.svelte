<script lang="ts">
    import Fa from "svelte-fa";
    import {faCircleNotch} from '@fortawesome/free-solid-svg-icons';
    import { onDestroy, onMount } from "svelte";
    import { push } from "svelte-spa-router";

    export let size = 10;
    export let color = "#ff8655";
    export let entire = false;
    export let inElement = false;

    let timeout;

    onMount(() => {
        // timeout = setTimeout(() => push('/NotFound'), 3000);
    })
    onDestroy(() => clearTimeout(timeout));

//TODO: fix loader on app init
</script>

{#if inElement}
    <div class="spinner {entire ? 'whole':''}">
        <span class="rotation">
        <Fa icon={faCircleNotch} size={`${size}x`} color={color}/>
        </span>
    </div>
{:else}
<div class="loader {entire ? 'whole':''}">
    <div class="spinner">
        <Fa icon={faCircleNotch} size={`${size}x`} color={color} />
    </div>
</div>
{/if}

<style>
    .whole {
        background-color: white;
    }

    .loader {
        z-index: 1000;
        width: 100vw;
        height: 100vh;
        position: fixed;
        top: 0;
        left: 0;
        display: flex;
        justify-content: center; 
        align-items: center;
    }
    .spinner {
        z-index: 2;
        width: 100%;
        height: 100%;
        position: absolute;
        display: flex;
        justify-content: center;
        align-items: center;
    }
    .rotation {
        animation: 1s linear spin infinite;
        width: auto;
        display: inline;
    }
    
    @keyframes spin {
        0% {transform: rotate(0);}
        50% {transform: rotate(180deg);}
        100% {transform: rotate(360deg);}
    }
</style>