<script lang="ts">
    import Fa from "svelte-fa";
    import {faCircleNotch} from '@fortawesome/free-solid-svg-icons';
    import { onDestroy, onMount } from "svelte";
import { push } from "svelte-spa-router";

    export let size = 10;
    export let color = "#ff8655";
    export let entire = true;
    export let inElement = false;

    let timeout;

    onMount(() => {
        timeout = setTimeout(() => push('/NotFound'), 3000);
    })
    onDestroy(() => clearTimeout(timeout));

</script>

{#if inElement}
<div class='loader-inelement'>
    <div class="spinner">
        <Fa icon={faCircleNotch} size={`${size}x`} color={color}/>
    </div>
</div>
{:else}
<div class={`loader ${entire ? 'whole':''}`}>
    <div class="spinner">
        <Fa icon={faCircleNotch} size={`${size}x`} color={color} />
    </div>
</div>
{/if}

<style>
    .whole {
        background-color: white;
    }
    .loader-inelement {
        width: auto;
        /* height: auto; */
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
        width: auto;
        height: auto;
        animation: 1s linear spin infinite;
    }
    
    @keyframes spin {
        0% {transform: rotate(0);}
        50% {transform: rotate(180deg);}
        100% {transform: rotate(360deg);}
    }
</style>