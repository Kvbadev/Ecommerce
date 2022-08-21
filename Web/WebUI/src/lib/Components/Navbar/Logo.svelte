<script lang="ts">
import { link } from "svelte-spa-router";



    const imgsrc = "src/assets/svelte.svg"

</script>

<li>
    <a href="/" use:link><img src={imgsrc} alt="website logo" /></a>
</li>

<style>
    img {
        height: 3vw;
        width: 3vw;
    }
    li {
        position: absolute;
        left: 50%;
        transform: translateX(-50%);
        cursor: pointer;
    }
</style>