const int Z_POT_PIN = A1;
const int X_POT_PIN = A2;
const int BUTTON_PIN = 2;

void setup() {
  Serial.begin(9600);
  pinMode(X_POT_PIN, INPUT);
  pinMode(Z_POT_PIN, INPUT);
  pinMode(BUTTON_PIN, INPUT);
  while (Serial.available() <= 0) {
    Serial.println("waiting...");
    delay(300);
  }
}

int clamp(int input) {
  if (input < 0) {
    return 0;
  }
  if (input > 100) {
    return 100;
  }
  return input;
}

void loop() {
  if (!Serial.available()) return;

  int zVal = analogRead(Z_POT_PIN);
  int mappedZ = clamp(map(zVal, 480, 1024, 0, 100));

  int xVal = analogRead(X_POT_PIN);
  int mappedX = clamp(map(xVal, 480, 1024, 0, 100));

  int buttonState = digitalRead(BUTTON_PIN);

  Serial.print(mappedX);
  Serial.print(',');
  Serial.print(mappedZ);
  Serial.print(',');
  Serial.print(buttonState);
  Serial.println();

  delay(50);
}
