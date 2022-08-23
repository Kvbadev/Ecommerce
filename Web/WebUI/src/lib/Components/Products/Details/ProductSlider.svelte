<script lang="ts">
    export let src = []
</script>

<div class="slider">
    <img src={src[0]} alt="Product photos"/>
</div>

<style>
    .slider{
        background-color: beige;
        grid-area: slider;
        width: 50vw;
    }

</style>