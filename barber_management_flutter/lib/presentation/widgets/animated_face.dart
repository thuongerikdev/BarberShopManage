import 'package:flutter/material.dart';

class AnimatedFace extends StatelessWidget {
  const AnimatedFace({super.key});

  @override
  Widget build(BuildContext context) {
    return Container(
      width: 100,
      height: 100,
      color: Colors.blue,
      child: Center(child: Text("Animated Face")),
    );
  }
}