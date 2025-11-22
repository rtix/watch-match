<template>
  <PinInput.Root>
    <PinInput.Label><h2>Enter room code:</h2></PinInput.Label>
    <PinInput.Control class="pin-input__control">
      <PinInput.Input v-for="id in [0, 1, 2 ,3]" :key="id" :index="id" class="pin-input__input"/>
    </PinInput.Control>
    <PinInput.HiddenInput />
  </PinInput.Root>
</template>

<script setup lang="ts">
import { PinInput } from '@ark-ui/vue/pin-input'
</script>

<style scoped>
.pin-input__control {
  display: flex;
  justify-content: center;
}

.pin-input__input {
  width: 2rem;
  font-size: 2rem;
  text-align: center;
}
</style>
