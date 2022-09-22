<script lang="ts">

import { agent } from "../../Utils/agent";

import { onMount } from "svelte";
import { client, hostedFields} from 'braintree-web';
  import Loader from '../Common/Loader.svelte';
  import {oneTimeProduct} from '../../Stores/stores';
  import { get } from "svelte/store";
  import { toast } from "@zerodevx/svelte-toast";
  import { push } from "svelte-spa-router";
  import { removeShoppingCart } from "../../Stores/ShoppingCartExtensions";

let fields: {
    cardholderName: HTMLElement,
    number: HTMLElement,
    expirationDate: HTMLElement,
    cvv: HTMLElement
} = {} as any;
let loading = true, submitting, getPayload;

async function handleSubmit(event: MouseEvent) {
    const payload = await getPayload(event);
    if(payload === null){
        console.error("Could not obtain payload");
        submitting = false;
        return;
    }
    const res = !$oneTimeProduct?.id ?
    await agent.PaymentGateway.BuyCart(payload.nonce) :
    await agent.PaymentGateway.BuyProduct(payload.nonce, get(oneTimeProduct));

    if(res === null){
        console.error("Could not finalize the transaction");
        submitting = false;
        return;
    }
    submitting = false;
    toast.push('Transaction was successfull🎉', {theme: {'--toastBackground': '#90EE90'}});
    await removeShoppingCart();
    push('/');
}

onMount(async () => {
    const token = await agent.PaymentGateway.GetToken();
    if(token === null) return;

    //return an instance of hostedFileds that are furher used to obtain a transaction nonce
    const instance = await client.create({
        authorization: token
    }).then(cl => {
        return hostedFields.create( {
            client: cl,
            authorization: token,
            fields: {
                cardholderName: {
                    container: fields.cardholderName,
                    placeholder: 'Name as it appears on your card'
                },
                number: {
                    container: fields.number,
                    placeholder: '4111 1111 1111 1111'
                },
                expirationDate: {
                    container: fields.expirationDate,
                    placeholder: '02/22'
                },
                cvv: {
                    container: fields.cvv,
                    placeholder: '123'
                }
            }
        }).then(fields => {
            loading = false;
            return fields;
        })
    })
    getPayload = (e) => {
        submitting = true;
        e.preventDefault();
        let payld = null;
        return new Promise((res, rej) => {
            instance.tokenize((err, payload) => {
                if(err){
                    console.error(err);
                    rej(err);
                }
                //payload = nonce to process the transaction
                payld = payload;
                res(payld);
            })
        })
    }
}) 

</script>


<div class="container">
    {#if loading}
    <Loader inElement size={3} color='#000000' entire/>
    {/if}
    <form class="form">
        <div class="cardholder-name-div">
            <label for="cardholder-name">Cardholder Name</label>
            <div class="cardholder-name" bind:this={fields.cardholderName}></div>
        </div>
        <div class="card-number-div">
            <label for="card-number">Credit card number</label>
            <div class="card-number" bind:this={fields.number}></div>
        </div>
        <div class="exp-cvv-div">
            <label for="expiration">Expiration</label>
            <div class="expiration" bind:this={fields.expirationDate}></div>
            <label for="cvv">CVV</label>
            <div class="cvv" bind:this={fields.cvv}></div>
        </div>
        <button class="submit" on:click={(e) => handleSubmit(e)}>
            {#if submitting}
            <Loader inElement size={1} entire />
            {/if}
            Submit
        </button>
    </form>
</div>


<style>
    .container {
        position: relative;
        width: 50rem;
        height: 25rem;
    }
    form {
        display: flex;
        flex-direction: column;
        align-items: center;
    }
    form > div {
        width: 45rem;
        justify-content: center;
        height: 6rem;
        display: flex;
    }
    .submit {
        width: 15rem;
        position: relative;
        height: 3rem;
        margin: 0;
        padding: 0;
        display: flex;
        justify-content: center;
        align-items: center;
    }
    form > div > div {
        border: 0.2rem black solid;
        padding: 1rem;
        display: grid;
    }
    form > div > label {
        width: 12rem;
        display: flex;
        justify-content: center;
        align-items: center;
    }
    form * {
        margin: 0.3rem;
    }
</style>