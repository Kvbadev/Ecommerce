<script lang="ts">
    import Fa from "svelte-fa";
    import {faCircleNotch} from '@fortawesome/free-solid-svg-icons';
    import { onDestroy, onMount } from "svelte";
    import { push } from "svelte-spa-router";

    export let size = 10;
    export let color = "#ff8655";
    export let inElement = false;
    export let bg = null;

    let timeout;

    if(inElement === false){
        onMount(() => {
            timeout = setTimeout(() => push('/NotFound'), 5000);
        })
        onDestroy(() => clearTimeout(timeout));
    }

</script>

{#if inElement}
    <div class="spinner" style="{bg ? 'background-color: '+bg : ''}">
        <span class="rotation">
            <Fa icon={faCircleNotch} size={`${size}x`} color={color} spin />
        </span>
    </div>
{:else}
<div class="loader">
    <div class="rotation">
        <Fa icon={faCircleNotch} size={`${size}x`} color={color} spin />
    </div>
</div>
{/if}

<!-- Loader need to have parent with position relative and display: flex! -->

<style>
    .loader {
        background-color: white;
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
        border-radius: inherit;
        background-color: inherit;
        z-index: 2;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        position: absolute;
        display: flex;
        justify-content: center;
        align-items: center;
    }
    .rotation {
        width: auto;
        display: flex;
    }
</style>