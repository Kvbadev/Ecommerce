<script type="typescript">
import type Product from "src/lib/Models/product";
import { agent } from "../../../Utils/agent";
import { onMount } from "svelte";
import { products } from "../../../Stores/stores";

import ProductParameters from "./ProductParameters.svelte";
import ProductSlider from "./ProductSlider.svelte";
import Loader from "../../Common/Loader.svelte";
import ProductPrice from "./ProductPrice.svelte";

export let params = {} as any;
let product;


</script>

{#if (product = $products?.find(x => x.id === params.id)) == undefined}
    <Loader entire />
{:else}
<div class="container">
    <ProductSlider product={product} />
    <ProductPrice params={params} product={product} />
    <ProductParameters product={product} />
</div> 
{/if}

<style>
    .container {
        min-width: 108rem;
        width: 100vw;
        height: calc(100vh - 4.2vw);
        overflow: scroll;
        background-color: white;
        display: grid;
        grid-template: 
        'slider slider slider price price price'
        'slider slider slider params params params'
        'slider slider slider params params params';
    }
</style>