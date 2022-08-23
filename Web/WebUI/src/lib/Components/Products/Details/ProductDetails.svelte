<script type="typescript">
import type Product from "src/lib/Models/product";
import { agent } from "../../../Utils/agent";
import { onMount } from "svelte";
import { push } from "svelte-spa-router";
import { get } from "svelte/store";
import { products } from "../../../Stores/stores";

import ProductParameters from "./ProductParameters.svelte";
import ProductSlider from "./ProductSlider.svelte";
import Loader from "../../Common/Loader.svelte";
import ProductPrice from "./ProductPrice.svelte";

export let params = {} as any;

let product: Product | undefined;

async function getProduct() {
    product = get(products).find(v => v.id === params.id);
    if(!product){
        product = await agent.Products.getOne(params.id);
    }
}

onMount(async () => {
    await getProduct();
})

</script>

{#if !product}
    <Loader />
{:else}
<div class="container">
    <ProductSlider />
    <ProductPrice product={product}/>
    <ProductParameters product={product} />
</div>
{/if}

<style>
    .container {
        width: 100vw;
        height: calc(100vh - 4.2vw);

        display: grid;
        grid-template: 
        'slider slider slider price price price'
        'slider slider slider params params params'
        'slider slider slider params params params';
    }
</style>