<script type="typescript">
import { products } from "../../../Stores/stores";

import ProductParameters from "./ProductParameters.svelte";
import ProductSlider from "./ProductSlider.svelte";
import Loader from "../../Common/Loader.svelte";
import ProductPrice from "./ProductPrice.svelte";
  import { onMount } from "svelte";
  import { agent } from "../../../Utils/agent";

export let params = {} as any;
let product = null;

    onMount(async () => {
        product = $products?.find(x => x.id === params.id)
        if(!product) {
            product = await agent.Products.getOne(params.id);
        }
    })


</script>

{#if !product}
    <Loader />
{:else}
<div class="container">
    <ProductSlider product={product} />
    <ProductPrice product={product} />
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