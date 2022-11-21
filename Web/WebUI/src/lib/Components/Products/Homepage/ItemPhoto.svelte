<script lang="ts">
  import Loader from "../../Common/Loader.svelte";
export let src, alt;
</script>
<span>
    {#if src?.loaded !== true}
        <Loader inElement size={2} bg="white" color={'#000000'}/>
    {/if}
    <img src={src} alt={alt} on:load={() => {src = new String(src); src.loaded = true}}/>
</span>

<style>
    span {
        width: auto;
        height: auto;
        position: relative;
    }
    img {
        width: 14vw;
        height: 14vw;
        border-radius: 9rem;
    }
</style>