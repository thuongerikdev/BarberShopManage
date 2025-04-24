import 'package:barbermanagemobile/data/models/promotion_model.dart';

abstract class PromotionRepository {
  Future<List<PromotionModel>> getPromotions();
}