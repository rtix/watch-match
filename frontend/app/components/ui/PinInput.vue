<template>
  <PinInput.RootProvider :value="pinInput">
    <PinInput.Label>
      <h2>Enter room code:</h2>
    </PinInput.Label>
    <PinInput.Control class="pin-input__control">
      <PinInput.Input
        v-for="id in [0, 1, 2, 3]"
        :key="id"
        :index="id"
        class="pin-input__input"
      />
    </PinInput.Control>
    <PinInput.HiddenInput />
  </PinInput.RootProvider>
</template>

<script setup lang="ts">
import { PinInput, usePinInput } from "@ark-ui/vue/pin-input";
const pinInput = usePinInput({
  placeholder: "",
  autoFocus: true,
  onValueComplete: handleComplete,
  onValueChange: (v) => emit("value-change", v),
});

const emit = defineEmits<{
  complete: [pin: string];
  "value-change": [PinInput.ValueChangeDetails];
}>();

function handleComplete({ valueAsString }: { valueAsString: string }) {
  emit("complete", valueAsString);
}

defineExpose({
  clear: pinInput.value.clearValue,
});
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
