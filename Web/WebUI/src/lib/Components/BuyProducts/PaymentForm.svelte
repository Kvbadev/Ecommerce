<script lang="ts">

import { agent } from "../../Utils/agent";

import { onMount } from "svelte";
import { client, HostedFields, hostedFields, HostedFieldsEvent} from 'braintree-web';
import Loader from '../Common/Loader.svelte';
import {oneTimeProduct, shoppingCart} from '../../Stores/stores';
import { get } from "svelte/store";
import { toast } from "@zerodevx/svelte-toast";
import { push } from "svelte-spa-router";
import { removeShoppingCart } from "../../Stores/ShoppingCartExtensions";
import type { HostedFieldsHostedFieldsFieldData } from "braintree-web/modules/hosted-fields";

let fields: {
    cardholderName: HTMLElement, 
    number: HTMLElement, 
    expirationDate: HTMLElement, 
    cvv: HTMLElement
} = {} as any;
//TODO: add paypal gateway 
//TODO: add delivery

let loading = true, submitting, getPayload, instance: HostedFields, canSubmit: any=false;


async function handleSubmit(event: MouseEvent) {
    event.preventDefault();
    if(!canSubmit){
        toast.push("You have to fill the payment form with proper values!"); //change width
        return;
    } 
    if($shoppingCart.items.length === 0 && $oneTimeProduct === null){
        toast.push("You have to select products firstly!");
        return;
    }
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
    try{
        instance = await client.create({
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
    } catch (err){
        console.error(err);
        loading = false;
    } 
    const onInputChange = (event: HostedFieldsEvent) => {
        const field:HostedFieldsHostedFieldsFieldData = event.fields[event.emittedBy];
        if(!field.isValid && !field.isEmpty) {
            field.container.dataset.valid = 'false';
        }
        else if(field.isEmpty){
            canSubmit = false;
            return;
        }
        else {
            field.container.dataset.valid = 'true';
        }
        fields = fields; //triggering reactivity

        canSubmit = 
        (Object.values(fields).filter(x => x.dataset.valid === "false").length===0) &&
        Object.values(event.fields).filter(x => x.isEmpty).length===0;
    }
    instance.on('blur', e => onInputChange(e));
    instance.on('validityChange', e => onInputChange(e));
    instance.on('empty', e => onInputChange(e));
    
    getPayload = (e) => {
        submitting = true;
        let payld = null;
        return new Promise((res, rej) => {
            instance.tokenize((err, payload) => {
                if(err){
                    console.error(err);
                    submitting = false;
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
    <form class="form">
        <span>
        {#if loading}
        <Loader inElement size={3} color='#000000' entire/>
        {/if}
        <div class="cardholder-name-div form-div">
            <label for="cardholer-name" 
                class={ fields.cardholderName?.dataset.valid === "true" ? '' : 'label-error'}>
                Cardholder Name
            </label>
            <div class="cardholder-name" bind:this={fields.cardholderName} data-valid="true">
            </div>
        </div>
        <div class="card-number-div form-div">
            <label for="card-number" 
                class={ fields.number?.dataset.valid === "true" ? '' : 'label-error'}>
                Credit card number
            </label>
            <div class="card-number " bind:this={fields.number} data-valid="true">
            </div>
        </div>
        <div class="exp-cvv-div form-div">
            <div class="exp-div">
                <label for="expiration" 
                    class={ fields.expirationDate?.dataset.valid === "true" ? '' : 'label-error'}>
                    Expiration Date
                </label>
                <div class="expiration" bind:this={fields.expirationDate} data-valid="true">
                </div>
            </div>
            <div class="cvv-div">
                <label for="cvv" class={ fields.cvv?.dataset.valid === "true" ? '' : 'label-error'}>
                    CVV
                </label>
                <div class="cvv" bind:this={fields.cvv} data-valid="true">
                </div>
            </div>
        </div>
        <div class="submit-div">
            <button class='submit' 
                on:click={(e) => handleSubmit(e)}>

                {#if submitting}
                <Loader inElement size={1} color="#ffffff"/>
                {/if}
                Submit
            </button>
        </div>
        </span> 
    </form>
</div>


<style>
    .container {
        margin: 0 2rem;
        width: calc(50vw - 2rem);
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .form-div, .exp-cvv-div > div{
        width: 100%;
        height: 12rem;
        display: flex;
        flex-direction: column;
        justify-content: space-evenly;
        align-items: center;
        border: none;
    }
    .exp-cvv-div > div {
        width: 20rem;
        padding: 0;
    }
    .submit-div {
        width: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
    }
    .submit {
        width: 25rem;
        height: 4.5rem;
        margin: 0;
        padding: 0;
        display: flex;
        justify-content: center;
        align-items: flex-end;
        position: relative;
        border: none;
        border-radius: 1rem;
        font-size: 2.5rem;
        font-family: 'Anek Telugu';
        background-color: rgb(15, 193, 15);
        cursor: pointer;
    }
    form > span > div > div, .exp-cvv-div > div > div {
        border: 0.2rem black solid;
        border-radius: 0.5rem;
        padding: 1rem;
        height: 6.5rem;
    }

    .exp-cvv-div {
        display: flex;
        flex-direction: row;
        width: 100%;
        justify-content: space-evenly;
    }

    label {
        font-family: 'Roboto slab';
        font-size: 2rem;
        height: 1.5rem;
        display: flex;
        justify-content: center;
        align-items: center;
        transition: all 100ms;
    }
    form {
        width: 80%;
        min-height: 48rem;
        height: 100%;
        border-radius: 2rem;
        border: 0.2rem solid black;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    form * {
        margin: 0.3rem;
    }
    span {
        position: relative;
    }

</style>